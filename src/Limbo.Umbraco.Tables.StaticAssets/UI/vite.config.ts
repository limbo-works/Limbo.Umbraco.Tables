import { defineConfig } from "vite";
import postcss from 'rollup-plugin-postcss';
import postcssLit from 'rollup-plugin-postcss-lit';
export default defineConfig({
    build: {
        lib: {
            entry: "src/entryPoint.ts", // your web component source file
            formats: ["es"],
        },
        outDir: "../wwwroot", // all compiled files will be placed here
        emptyOutDir: true,
        sourcemap: true,
        rollupOptions: {
            external: [/^@umbraco/, ], // ignore the Umbraco Backoffice package in the build,
           
        },
    },
    base: "/App_Plugins/Limbo.Umbraco.Tables.StaticAssets/",
    css: {
        preprocessorOptions: {
            less: {
                math: "always",
                relativeUrls: true,
                javascriptEnabled: true,
            },
        },
    },
    plugins: [
        postcss({
            // ...
        }),
        postcssLit({
            importPackage: 'lit-element',
        }),
    ],
});
