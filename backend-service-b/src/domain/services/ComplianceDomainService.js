/**
 * Domain Service: ComplianceDomainService (service-b)
 */
class ComplianceDomainService {
  static maskCardholderPan(pan) {
    if (typeof pan !== 'string') return '';
    const clean = pan.replace(/[\s-]/g, '');
    if (clean.length < 13 || clean.length > 19) return clean;
    const first6 = clean.slice(0, 6);
    const last4 = clean.slice(-4);
    const maskedMiddle = '*'.repeat(clean.length - 10);
    return `${first6}${maskedMiddle}${last4}`;
  }

  static maskStudentPii(text) {
    if (typeof text !== 'string') return '';
    const ssnMasked = text.replace(/\b\d{3}-\d{2}-\d{4}\b/g, 'XXX-XX-XXXX');
    return ssnMasked.replace(/([a-zA-Z0-9_\.-]+)@([a-zA-Z0-9\.-]+)/g, (match, user, domain) => {
      return `${user[0]}***@${domain}`;
    });
  }
}

module.exports = { ComplianceDomainService };
