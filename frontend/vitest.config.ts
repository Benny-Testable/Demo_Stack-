import { defineConfig } from 'vitest/config';
import * as fs from 'node:fs';
import * as path from 'node:path';

function angularInlinePlugin() {
  return {
    name: 'angular-inline-plugin',
    transform(code: string, id: string) {
      if (!id.endsWith('.ts')) return null;
      let transformed = code;
      transformed = transformed.replace(/templateUrl:\s*['"]([^'"]+)['"]/g, (_match, url) => {
        const fullPath = path.resolve(path.dirname(id), url);
        if (fs.existsSync(fullPath)) {
          const content = fs.readFileSync(fullPath, 'utf-8');
          return 'template: ' + JSON.stringify(content);
        }
        return 'template: ""';
      });
      transformed = transformed.replace(/styleUrl:\s*['"]([^'"]+)['"]/g, 'styles: []');
      transformed = transformed.replace(/styleUrls:\s*\[[^\]]*\]/g, 'styles: []');
      return { code: transformed };
    },
  };
}

export default defineConfig({
  plugins: [angularInlinePlugin()],
  test: {
    environment: 'jsdom',
    globals: true,
    coverage: {
      provider: 'v8',
      reporter: ['json', 'json-summary', 'cobertura', 'text'],
      reportsDirectory: './coverage',
      exclude: ['scripts/**', 'karma.conf.js', 'eslint.config.js', 'vitest.config.ts'],
    },
  },
});
