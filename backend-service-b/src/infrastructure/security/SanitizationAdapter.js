const path = require('path');

class SanitizationAdapter {
  static sanitizeInputString(input) {
    if (typeof input !== 'string') return '';
    return input
      .replace(/[<>]/g, '')
      .replace(/javascript:/gi, '')
      .trim();
  }

  static resolveSafeFilePath(baseDir, userInputFilename) {
    if (typeof userInputFilename !== 'string') throw new Error('invalid filename');
    const safeFilename = path.basename(userInputFilename);
    const resolved = path.resolve(baseDir, safeFilename);
    if (!resolved.startsWith(path.resolve(baseDir))) {
      throw new Error('path traversal attempt detected');
    }
    return resolved;
  }
}

module.exports = { SanitizationAdapter };
