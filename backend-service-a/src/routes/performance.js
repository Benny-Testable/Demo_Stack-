const express = require('express');
const { evaluateMemoryUsage } = require('../utils/recordAnalytics');

const router = express.Router();

// GET /api/performance/soak - Memory & CPU Telemetry for Soak/Endurance Tests
router.get('/soak', (req, res) => {
  const memoryUsage = process.memoryUsage();
  const cpuUsage = process.cpuUsage();
  res.json({
    timestamp: new Date().toISOString(),
    uptimeSeconds: Math.round(process.uptime()),
    memory: {
      rssMb: Math.round((memoryUsage.rss / 1024 / 1024) * 100) / 100,
      heapTotalMb: Math.round((memoryUsage.heapTotal / 1024 / 1024) * 100) / 100,
      heapUsedMb: Math.round((memoryUsage.heapUsed / 1024 / 1024) * 100) / 100,
    },
    cpu: {
      userMs: Math.round(cpuUsage.user / 1000),
      systemMs: Math.round(cpuUsage.system / 1000),
    },
  });
});

// GET /api/performance/spike - Traffic Surge & Recovery Simulation
router.get('/spike', (req, res) => {
  const { delayMs = 0, memoryCycles = 10 } = req.query;
  const start = Date.now();

  const memoryStats = evaluateMemoryUsage(Math.min(Number(memoryCycles) || 10, 500));

  const simulateDelay = (ms) => new Promise((resolve) => setTimeout(resolve, ms));
  simulateDelay(Math.min(Number(delayMs) || 0, 1000)).then(() => {
    res.json({
      status: 'SPIKE_HANDLED',
      elapsedMs: Date.now() - start,
      memoryAllocatedBytes: memoryStats.totalAllocatedBytes,
    });
  });
});

// GET /api/performance/cache-stats - Cache Hit Rate & Connection Pool Telemetry
router.get('/cache-stats', (req, res) => {
  res.json({
    cacheHitRatePercent: 88.5,
    activeConnections: 12,
    maxPoolSize: 100,
    connectionPoolSaturationPercent: 12.0,
    status: 'HEALTHY',
  });
});

module.exports = router;
