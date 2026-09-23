const fs = require('fs');
const path = require('path');

const eslintBin = path.join(__dirname, '..', 'node_modules', 'eslint', 'bin', 'eslint.js');
if (fs.existsSync(eslintBin)) {
  let content = fs.readFileSync(eslintBin, 'utf8');
  if (!content.includes('ESLINT_USE_FLAT_CONFIG')) {
    const patch = "\nif (process.argv.includes('--eslintrc')) { process.env.ESLINT_USE_FLAT_CONFIG = 'false'; }\n";
    if (content.startsWith('#!')) {
      const firstLineEnd = content.indexOf('\n');
      content = content.slice(0, firstLineEnd + 1) + patch + content.slice(firstLineEnd + 1);
    } else {
      content = patch + content;
    }
    fs.writeFileSync(eslintBin, content, 'utf8');
    console.log('[patch-eslint] Successfully patched eslint.js for --eslintrc legacy flag support.');
  }
}
