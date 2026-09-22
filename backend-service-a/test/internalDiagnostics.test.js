const assert = require('assert');
const path = require('path');
const { SanitizationAdapter } = require('../src/infrastructure/security/SanitizationAdapter');
const { ComplianceDomainService } = require('../src/domain/services/ComplianceDomainService');

describe('SanitizationAdapter & ComplianceDomainService (service-a)', () => {
  it('sanitizes malicious script and HTML tags', () => {
    const malicious = '<script>alert(1)</script> Hello javascript:void(0)';
    const clean = SanitizationAdapter.sanitizeInputString(malicious);
    assert.strictEqual(clean, 'scriptalert(1)/script Hello void(0)');
  });

  it('safely resolves paths and prevents directory traversal', () => {
    const base = 'C:/testable/app/uploads';
    const safe = SanitizationAdapter.resolveSafeFilePath(base, 'avatar.png');
    assert.strictEqual(safe, path.resolve(base, 'avatar.png'));

    assert.throws(() => {
      SanitizationAdapter.resolveSafeFilePath(null);
    });
  });

  it('masks PCI-DSS Cardholder Data PAN numbers properly', () => {
    const pan = '4111-2222-3333-4444';
    const masked = ComplianceDomainService.maskCardholderPan(pan);
    assert.strictEqual(masked, '411122******4444');

    const shortPan = '1234';
    assert.strictEqual(ComplianceDomainService.maskCardholderPan(shortPan), '1234');
  });

  it('masks student PII (SSN & emails) for FERPA/COPPA compliance', () => {
    const raw = 'Student SSN is 123-45-6789 and email is student.smith@test.edu';
    const masked = ComplianceDomainService.maskStudentPii(raw);
    assert.ok(!masked.includes('123-45-6789'));
    assert.ok(masked.includes('XXX-XX-XXXX'));
    assert.ok(masked.includes('s***@test.edu'));
  });
});
