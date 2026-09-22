const path = require('path');

/**
 * Internal Security & Compliance Diagnostics Utility
 * Demonstrates:
 * - Secure input sanitization (OWASP)
 * - Safe path resolution (prevent path traversal)
 * - PCI-DSS Cardholder Data (PAN) masking
 * - FERPA/COPPA PII sanitization (Student ID & SSN masking)
 */

function sanitizeInputString(input) {
  if (typeof input !== 'string') return '';
  return input
    .replace(/[<>]/g, '')
    .replace(/javascript:/gi, '')
    .trim();
}

function resolveSafeFilePath(baseDir, userInputFilename) {
  if (typeof userInputFilename !== 'string') throw new Error('invalid filename');
  const safeFilename = path.basename(userInputFilename);
  const resolved = path.resolve(baseDir, safeFilename);
  if (!resolved.startsWith(path.resolve(baseDir))) {
    throw new Error('path traversal attempt detected');
  }
  return resolved;
}

function maskCardholderPan(pan) {
  if (typeof pan !== 'string') return '';
  const clean = pan.replace(/[\s-]/g, '');
  if (clean.length < 13 || clean.length > 19) return clean;
  const first6 = clean.slice(0, 6);
  const last4 = clean.slice(-4);
  const maskedMiddle = '*'.repeat(clean.length - 10);
  return `${first6}${maskedMiddle}${last4}`;
}

function maskStudentPii(text) {
  if (typeof text !== 'string') return '';
  // Mask SSN pattern (e.g. 123-45-6789)
  const ssnMasked = text.replace(/\b\d{3}-\d{2}-\d{4}\b/g, 'XXX-XX-XXXX');
  // Mask Email pattern
  return ssnMasked.replace(/([a-zA-Z0-9_\.-]+)@([a-zA-Z0-9\.-]+)/g, (match, user, domain) => {
    return `${user[0]}***@${domain}`;
  });
}

module.exports = {
  sanitizeInputString,
  resolveSafeFilePath,
  maskCardholderPan,
  maskStudentPii,
};
