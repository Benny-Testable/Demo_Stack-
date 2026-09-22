const assert = require('assert');
const path = require('path');
const { SanitizationAdapter } = require('../src/infrastructure/security/SanitizationAdapter');
const { ComplianceDomainService } = require('../src/domain/services/ComplianceDomainService');

describe('SanitizationAdapter & ComplianceDomainService (service-b)', () => {
  it('sanitizes malicious script and HTML tags', () => {
    const malicious = '<script>alert("xss")</script> Safe Text';
    const clean = SanitizationAdapter.sanitizeInputString(malicious);
    assert.strictEqual(clean, 'scriptalert("xss")/script Safe Text');
  });

  it('safely resolves paths and throws on null', () => {
    const base = 'C:/testable/storage';
    const safe = SanitizationAdapter.resolveSafeFilePath(base, 'report.pdf');
    assert.strictEqual(safe, path.resolve(base, 'report.pdf'));

    assert.throws(() => SanitizationAdapter.resolveSafeFilePath(null));
  });

  it('masks PCI-DSS Cardholder Data PAN numbers', () => {
    const pan = '5500-1111-2222-3333';
    assert.strictEqual(ComplianceDomainService.maskCardholderPan(pan), '550011******3333');
  });

  it('masks student PII (SSN & emails)', () => {
    const text = 'SSN: 999-00-1111, email: student@university.edu';
    const masked = ComplianceDomainService.maskStudentPii(text);
    assert.ok(!masked.includes('999-00-1111'));
    assert.ok(masked.includes('XXX-XX-XXXX'));
  });
});
