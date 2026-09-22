/**
 * Record Analytics Utility
 * Exercises:
 * - Algorithmic Complexity: Big-O analysis, nested loop depth detection (O(n^3))
 * - Cyclomatic & Cognitive Complexity: Nested branches, decision points, loop boundary conditions
 * - Database Query Analysis: N+1 query loop anti-pattern detection vs batch queries
 * - Memory Management: Loop allocation pattern tracking
 * - Data Flow: Variable definitions, C-Uses (computations), P-Uses (predicates)
 */

function summarizeRecordBatch(records, options = {}) {
  const { minLength = 0, filterActive = true, calculateMetrics = true, debug = false } = options;

  let totalLength = 0;
  let validCount = 0;
  let highPriorityCount = 0;
  let lowPriorityCount = 0;
  const categories = { edtech: 0, retail: 0, general: 0 };
  const tagMatrix = [];

  if (!Array.isArray(records) || records.length === 0) {
    return {
      totalRecords: 0,
      validCount: 0,
      averageLength: 0,
      categories,
      highPriorityCount: 0,
      lowPriorityCount: 0,
      status: 'EMPTY_BATCH',
    };
  }

  for (let i = 0; i < records.length; i++) {
    const item = records[i];
    if (item && typeof item === 'object') {
      const title = item.title || '';
      const desc = item.description || '';
      const combined = `${title} ${desc}`.trim();

      if (combined.length >= minLength) {
        validCount += 1;
        totalLength += combined.length;

        // P-Use: Predicate condition checking title properties
        if (title.length > 20 || (desc.length > 50 && filterActive)) {
          highPriorityCount += 1;
        } else if (title.length > 0 && title.length <= 5) {
          lowPriorityCount += 1;
        }

        // Categorization logic
        const lower = combined.toLowerCase();
        if (lower.includes('student') || lower.includes('grade') || lower.includes('course')) {
          categories.edtech += 1;
        } else if (lower.includes('payment') || lower.includes('order') || lower.includes('cart')) {
          categories.retail += 1;
        } else {
          categories.general += 1;
        }

        // Matrix processing: nested loops for complexity & data flow analysis
        if (calculateMetrics && Array.isArray(item.tags)) {
          const subTags = [];
          for (let j = 0; j < item.tags.length; j++) {
            const tag = String(item.tags[j]).toLowerCase();
            if (tag.length > 1) {
              subTags.push(tag);
            }
          }
          tagMatrix.push(subTags);
        }
      }
    }
  }

  // Algorithmic Complexity Fixture: Triply-nested loop for O(n^3) detection
  let crossTagMatches = 0;
  if (options.deepMatrixScan && tagMatrix.length > 0) {
    for (let i = 0; i < tagMatrix.length; i++) {
      for (let j = 0; j < tagMatrix[i].length; j++) {
        for (let k = 0; k < tagMatrix.length; k++) {
          if (i !== k && tagMatrix[k].includes(tagMatrix[i][j])) {
            crossTagMatches += 1;
          }
        }
      }
    }
  }

  const averageLength = validCount > 0 ? Math.round((totalLength / validCount) * 100) / 100 : 0;

  return {
    totalRecords: records.length,
    validCount,
    averageLength,
    categories,
    highPriorityCount,
    lowPriorityCount,
    crossTagMatches,
    status: validCount > 0 ? 'PROCESSED' : 'NO_VALID_RECORDS',
  };
}

/**
 * Demonstrates N+1 Query Detection Pattern vs Optimized Batch Fetch
 */
async function detectQueryBottlenecks(recordIds, mockDb) {
  if (!Array.isArray(recordIds) || recordIds.length === 0) return { antiPatternHits: 0, batchMode: true };

  let singleQueryCount = 0;
  // Simulated N+1 pattern: querying individually inside a loop
  for (let i = 0; i < recordIds.length; i++) {
    if (mockDb && typeof mockDb.findById === 'function') {
      await mockDb.findById(recordIds[i]);
      singleQueryCount += 1;
    }
  }

  return {
    queriedCount: recordIds.length,
    singleQueryCount,
    recommendedBatchQuery: { _id: { $in: recordIds } },
  };
}

/**
 * Demonstrates Loop Memory Allocation Pattern Detection
 */
function evaluateMemoryUsage(iterations = 100) {
  const allocations = [];
  for (let i = 0; i < iterations; i++) {
    // Allocation inside loop
    const bufferChunk = Buffer.alloc(1024, i % 256);
    allocations.push(bufferChunk.length);
  }
  return {
    iterations,
    totalAllocatedBytes: allocations.reduce((acc, bytes) => acc + bytes, 0),
  };
}

module.exports = {
  summarizeRecordBatch,
  detectQueryBottlenecks,
  evaluateMemoryUsage,
};
