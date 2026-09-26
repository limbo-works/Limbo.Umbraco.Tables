/**
 * Minimal RFC 4180 CSV reader.
 *
 * Handles quoted fields (`"a,b"`), escaped quotes (`""`), newlines inside quoted
 * fields, CRLF/LF line endings and a leading UTF-8 BOM. The delimiter is detected
 * automatically, as spreadsheet software in several locales exports semicolon
 * separated files rather than comma separated ones.
 */

const CANDIDATE_DELIMITERS = [",", ";", "\t", "|"];

/** Number of leading characters considered when detecting the delimiter. */
const DETECTION_SAMPLE_LENGTH = 65536;

/** Number of leading records considered when detecting the delimiter. */
const DETECTION_SAMPLE_RECORDS = 10;

function stripBom(text: string): string {
    return text.charCodeAt(0) === 0xfeff ? text.slice(1) : text;
}

/**
 * Splits `text` into records using the specified `delimiter`.
 *
 * Unquoted fields are trimmed, as leading and trailing whitespace around a
 * delimiter is rarely intentional. Quoted fields are returned verbatim.
 */
function tokenize(text: string, delimiter: string): string[][] {

    const records: string[][] = [];

    let record: string[] = [];
    let field = "";
    let quoted = false;
    let wasQuoted = false;
    let index = 0;

    const endField = () => {
        record.push(wasQuoted ? field : field.trim());
        field = "";
        wasQuoted = false;
    };

    const endRecord = () => {
        endField();
        records.push(record);
        record = [];
    };

    while (index < text.length) {

        const character = text[index];

        if (quoted) {
            if (character === "\"") {
                // A doubled quote inside a quoted field is an escaped quote
                if (text[index + 1] === "\"") {
                    field += "\"";
                    index += 2;
                    continue;
                }
                quoted = false;
                index++;
                continue;
            }
            field += character;
            index++;
            continue;
        }

        // A quote only opens a quoted field when it appears at the start of the field
        if (character === "\"" && field.length === 0) {
            quoted = true;
            wasQuoted = true;
            index++;
            continue;
        }

        if (character === delimiter) {
            endField();
            index++;
            continue;
        }

        if (character === "\r" || character === "\n") {
            endRecord();
            index += character === "\r" && text[index + 1] === "\n" ? 2 : 1;
            continue;
        }

        field += character;
        index++;

    }

    endRecord();

    // Drop records that are entirely empty - typically the trailing newline
    return records.filter(x => x.some(y => y.length > 0));

}

/**
 * Determines which delimiter `text` most likely uses.
 *
 * A real delimiter splits every record into the same number of columns, so each
 * candidate is scored by its most common column count weighted by how many of the
 * sampled records actually agree on it. Weighting by agreement matters: a comma is
 * a decimal separator in several locales, and would otherwise beat the semicolon
 * that actually separates the columns by shredding a few records into many fields.
 */
export function detectDelimiter(text: string): string {

    const sample = text.slice(0, DETECTION_SAMPLE_LENGTH);

    let bestDelimiter = CANDIDATE_DELIMITERS[0];
    let bestScore = 0;

    for (const delimiter of CANDIDATE_DELIMITERS) {

        const records = tokenize(sample, delimiter).slice(0, DETECTION_SAMPLE_RECORDS);
        if (records.length === 0) continue;

        const widths = records.map(x => x.length);

        // Find the most common column count, preferring the wider one on a tie
        const occurrences = new Map<number, number>();
        for (const width of widths) occurrences.set(width, (occurrences.get(width) ?? 0) + 1);

        let commonWidth = 0;
        let commonCount = 0;
        for (const [width, count] of occurrences) {
            if (count > commonCount || (count === commonCount && width > commonWidth)) {
                commonWidth = width;
                commonCount = count;
            }
        }

        // A delimiter that never splits a record is not a delimiter
        if (commonWidth < 2) continue;

        const score = commonWidth * (commonCount / widths.length);

        if (score > bestScore) {
            bestScore = score;
            bestDelimiter = delimiter;
        }

    }

    return bestDelimiter;

}

/**
 * Parses `text` into a rectangular grid of cell values.
 *
 * Short records are padded with empty strings so every returned record has the
 * same length, matching the table editor's model.
 *
 * @param text The CSV content to parse.
 * @param delimiter The delimiter to use. Detected automatically when not specified.
 * @returns A rectangular array of records, or an empty array if `text` holds no data.
 */
export function parseCsv(text: string, delimiter?: string): string[][] {

    const input = stripBom(text);
    if (input.length === 0) return [];

    const records = tokenize(input, delimiter ?? detectDelimiter(input));
    if (records.length === 0) return [];

    const width = records.reduce((max, record) => Math.max(max, record.length), 0);

    return records.map(record => record.length === width
        ? record
        : [...record, ...new Array<string>(width - record.length).fill("")]);

}
