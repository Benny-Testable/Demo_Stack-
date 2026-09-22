const assert = require('assert');
const http = require('http');
const { createApp } = require('../src/app');

function withServer(app, fn) {
  const server = http.createServer(app);
  return new Promise((resolve, reject) => {
    server.listen(0, () => {
      const { port } = server.address();
      fn(port)
        .then(resolve, reject)
        .finally(() => server.close());
    });
  });
}

function requestJson(port, path, method = 'GET', payload = null) {
  return new Promise((resolve, reject) => {
    const opts = {
      host: '127.0.0.1',
      port,
      path,
      method,
      headers: { 'content-type': 'application/json' },
    };
    const req = http.request(opts, (res) => {
      let body = '';
      res.on('data', (chunk) => { body += chunk; });
      res.on('end', () => {
        let parsed = null;
        try {
          parsed = JSON.parse(body);
        } catch (_) {
          parsed = body;
        }
        resolve({ status: res.statusCode, headers: res.headers, body: parsed });
      });
    });
    req.on('error', reject);
    if (payload) req.write(JSON.stringify(payload));
    req.end();
  });
}

describe('compliance routes (service-a)', () => {
  it('enforces security headers on every response', async () => {
    const app = createApp();
    const { headers } = await withServer(app, (port) => requestJson(port, '/health'));

    assert.strictEqual(headers['x-content-type-options'], 'nosniff');
    assert.strictEqual(headers['x-frame-options'], 'DENY');
    assert.ok(headers['strict-transport-security']);
  });

  it('GET /api/compliance/privacy serves GDPR privacy notice', async () => {
    const app = createApp();
    const { status, body } = await withServer(app, (port) => requestJson(port, '/api/compliance/privacy'));
    assert.strictEqual(status, 200);
    assert.strictEqual(body.status, 'ACTIVE');
    assert.ok(body.policy.includes('GDPR'));
  });

  it('GET /api/compliance/gdpr/export/:userId serves DSAR data export', async () => {
    const app = createApp();
    const { status, body } = await withServer(app, (port) => requestJson(port, '/api/compliance/gdpr/export/user-99'));
    assert.strictEqual(status, 200);
    assert.strictEqual(body.userId, 'user-99');
    assert.ok(body.dataCategories.includes('profile'));
  });

  it('DELETE /api/compliance/gdpr/erase/:userId executes Right to Erasure', async () => {
    const app = createApp();
    const { status, body } = await withServer(app, (port) => requestJson(port, '/api/compliance/gdpr/erase/user-99', 'DELETE'));
    assert.strictEqual(status, 200);
    assert.strictEqual(body.status, 'ERASED');
  });

  it('POST /api/compliance/coppa/consent validates parental consent rules', async () => {
    const app = createApp();
    // Denied if under 13 without consent
    const denied = await withServer(app, (port) =>
      requestJson(port, '/api/compliance/coppa/consent', 'POST', {
        parentEmail: 'parent@test.com',
        studentAge: 11,
        consentGranted: false,
      }),
    );
    assert.strictEqual(denied.status, 403);

    // Allowed with consent
    const allowed = await withServer(app, (port) =>
      requestJson(port, '/api/compliance/coppa/consent', 'POST', {
        parentEmail: 'parent@test.com',
        studentAge: 11,
        consentGranted: true,
      }),
    );
    assert.strictEqual(allowed.status, 200);
    assert.strictEqual(allowed.body.consentVerified, true);
  });

  it('POST /api/compliance/pci/mask-card masks cardholder PANs', async () => {
    const app = createApp();
    const { status, body } = await withServer(app, (port) =>
      requestJson(port, '/api/compliance/pci/mask-card', 'POST', {
        cardNumber: '4111222233334444',
      }),
    );
    assert.strictEqual(status, 200);
    assert.strictEqual(body.maskedCardNumber, '411122******4444');
    assert.strictEqual(body.compliance, 'PCI-DSS-v4.0');
  });

  it('GET /api/compliance/soc2/audit-logs returns audit records', async () => {
    const app = createApp();
    const { status, body } = await withServer(app, (port) => requestJson(port, '/api/compliance/soc2/audit-logs'));
    assert.strictEqual(status, 200);
    assert.strictEqual(body.complianceStandard, 'SOC2-Type-II');
    assert.ok(Array.isArray(body.logs));
  });
});
