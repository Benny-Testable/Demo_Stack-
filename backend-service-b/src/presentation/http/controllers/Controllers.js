const { ElasticsearchAdapter } = require('../../../infrastructure/search/ElasticsearchAdapter');
const { SearchRecordsUseCase } = require('../../../application/use-cases/RecordUseCases');

const esAdapter = new ElasticsearchAdapter();
const searchRecordsUseCase = new SearchRecordsUseCase({ elasticsearchAdapter: esAdapter });

class SearchController {
  static async search(req, res) {
    const q = req.query.q;
    if (!q) {
      return res.status(400).json({ error: 'query parameter q is required' });
    }
    try {
      const hits = await searchRecordsUseCase.execute(q);
      res.json(hits);
    } catch (err) {
      console.error('[service-b][search] error searching elasticsearch', err.message);
      res.status(502).json({ error: 'search service unavailable' });
    }
  }
}

class HealthController {
  static check(req, res) {
    res.json({ status: 'ok', service: 'backend-service-b' });
  }
}

module.exports = { SearchController, HealthController };
