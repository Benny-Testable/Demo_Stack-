const express = require('express');
const cors = require('cors');
const {
  securityHeadersMiddleware,
  errorHandlerMiddleware,
} = require('./presentation/http/middlewares/HttpMiddlewares');
const recordRoutes = require('./presentation/http/routes/record.routes');
const complianceRoutes = require('./presentation/http/routes/compliance.routes');
const performanceRoutes = require('./presentation/http/routes/performance.routes');

function createApp() {
  const app = express();

  app.use(securityHeadersMiddleware);
  app.use(cors());
  app.use(express.json());

  app.get('/health', (req, res) => res.json({ status: 'ok', service: 'backend-service-a' }));
  app.get('/privacy', (req, res) => res.redirect('/api/compliance/privacy'));

  app.use('/api/records', recordRoutes);
  app.use('/api/compliance', complianceRoutes);
  app.use('/api/performance', performanceRoutes);

  app.use(errorHandlerMiddleware);

  return app;
}

module.exports = { createApp };
