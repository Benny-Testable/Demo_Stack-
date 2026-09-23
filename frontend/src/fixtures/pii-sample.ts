// PII-shaped FIXTURE DATA for detector tests (presidio / semgrep-pii).
// Entirely fabricated; excluded from lint and coverage.
export const PII_FIXTURES = {
  customers: [
    { name: 'Alex Doe', email: 'alex.doe@example.com', phone: '+1-202-555-0143' },
    { name: 'Priya Kumar', email: 'priya.kumar@example.org', phone: '+44 20 7946 0958' },
  ],
  logLines: [
    'INFO user alex.doe@example.com signed in from 203.0.113.42',
    'WARN card ending 4242 declined for priya.kumar@example.org',
  ],
};
