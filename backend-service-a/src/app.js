const express = require('express');
const cors = require('cors');
const recordsRouter = require('./routes/records');
const complianceRouter = require('./routes/compliance');
const performanceRouter = require('./routes/performance');
const { securityHeadersMiddleware, errorHandlerMiddleware } = require('./middleware/security');

function createApp() {
  const app = express();
  app.use(securityHeadersMiddleware);
  app.use(cors());
  app.use(express.json());

  app.get('/health', (req, res) => res.json({ status: 'ok', service: 'backend-service-a' }));
  app.get('/privacy', (req, res) => res.redirect('/api/compliance/privacy'));

  app.use('/api/records', recordsRouter);
  app.use('/api/compliance', complianceRouter);
  app.use('/api/performance', performanceRouter);

  app.use(errorHandlerMiddleware);

  return app;
}

module.exports = { createApp };

