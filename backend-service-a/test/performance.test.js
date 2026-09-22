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

function getJson(port, path) {
  return new Promise((resolve, reject) => {
    http.get({ host: '127.0.0.1', port, path }, (res) => {
      let body = '';
      res.on('data', (c) => { body += c; });
      res.on('end', () => resolve({ status: res.statusCode, body: JSON.parse(body) }));
    }).on('error', reject);
  });
}

describe('performance telemetry routes (service-a)', () => {
  it('GET /api/performance/soak reports memory and CPU stats', async () => {
    const app = createApp();
    const { status, body } = await withServer(app, (port) => getJson(port, '/api/performance/soak'));

    assert.strictEqual(status, 200);
    assert.ok(body.uptimeSeconds >= 0);
    assert.ok(body.memory.rssMb > 0);
    assert.ok(body.memory.heapTotalMb > 0);
  });

  it('GET /api/performance/spike simulates and tracks spike performance', async () => {
    const app = createApp();
    const { status, body } = await withServer(app, (port) => getJson(port, '/api/performance/spike?delayMs=10&memoryCycles=5'));

    assert.strictEqual(status, 200);
    assert.strictEqual(body.status, 'SPIKE_HANDLED');
    assert.ok(body.elapsedMs >= 10);
  });

  it('GET /api/performance/cache-stats provides cache and pool metrics', async () => {
    const app = createApp();
    const { status, body } = await withServer(app, (port) => getJson(port, '/api/performance/cache-stats'));

    assert.strictEqual(status, 200);
    assert.strictEqual(body.status, 'HEALTHY');
    assert.strictEqual(body.cacheHitRatePercent, 88.5);
  });
});
