const { RecordAnalyticsDomainService } = require('../../../domain/services/RecordAnalyticsDomainService');

class PerformanceController {
  static getSoakTelemetry(req, res) {
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
  }

  static simulateSpike(req, res) {
    const { delayMs = 0, memoryCycles = 10 } = req.query;
    const start = Date.now();
    const memoryStats = RecordAnalyticsDomainService.evaluateMemoryUsage(Math.min(Number(memoryCycles) || 10, 500));

    const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms));
    delay(Math.min(Number(delayMs) || 0, 1000)).then(() => {
      res.json({
        status: 'SPIKE_HANDLED',
        elapsedMs: Date.now() - start,
        memoryAllocatedBytes: memoryStats.totalAllocatedBytes,
      });
    });
  }

  static getCacheStats(req, res) {
    res.json({
      cacheHitRatePercent: 88.5,
      activeConnections: 12,
      maxPoolSize: 100,
      connectionPoolSaturationPercent: 12.0,
      status: 'HEALTHY',
    });
  }
}

module.exports = { PerformanceController };
