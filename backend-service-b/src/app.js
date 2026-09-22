const express = require('express');
const cors = require('cors');
const { HealthController } = require('./presentation/http/controllers/Controllers');
const searchRouter = require('./presentation/http/routes/search.routes');

function createApp() {
  const app = express();
  app.use(cors());
  app.use(express.json());

  app.get('/health', HealthController.check);
  app.use('/api/search', searchRouter);

  return app;
}

module.exports = { createApp };
