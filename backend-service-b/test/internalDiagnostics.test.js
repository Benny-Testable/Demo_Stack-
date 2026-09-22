const assert = require('assert');
const path = require('path');
const {
  sanitizeInputString,
  resolveSafeFilePath,
  maskCardholderPan,
  maskStudentPii,
} = require('../src/utils/internalDiagnostics');

describe('internalDiagnostics utility (service-b)', () => {
  it('sanitizes malicious script and HTML tags', () => {
    const malicious = '<script>alert("xss")</script> Safe Text';
    const clean = sanitizeInputString(malicious);
    assert.strictEqual(clean, 'scriptalert("xss")/script Safe Text');
  });

  it('safely resolves paths and throws on null', () => {
    const base = 'C:/testable/storage';
    const safe = resolveSafeFilePath(base, 'report.pdf');
    assert.strictEqual(safe, path.resolve(base, 'report.pdf'));

    assert.throws(() => resolveSafeFilePath(null));
  });

  it('masks PCI-DSS Cardholder Data PAN numbers', () => {
    const pan = '5500-1111-2222-3333';
    assert.strictEqual(maskCardholderPan(pan), '550011******3333');
  });

  it('masks student PII (SSN & emails)', () => {
    const text = 'SSN: 999-00-1111, email: student@university.edu';
    const masked = maskStudentPii(text);
    assert.ok(!masked.includes('999-00-1111'));
    assert.ok(masked.includes('XXX-XX-XXXX'));
  });
});
