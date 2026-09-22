const express = require('express');
const { PerformanceController } = require('../controllers/PerformanceController');

const router = express.Router();

router.get('/soak', PerformanceController.getSoakTelemetry);
router.get('/spike', PerformanceController.simulateSpike);
router.get('/cache-stats', PerformanceController.getCacheStats);

module.exports = router;
