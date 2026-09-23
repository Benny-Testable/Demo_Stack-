// Intentional clone pair (duplicate-a.ts / duplicate-b.ts) so duplication
// detectors report a real match. Keep both in sync if edited.
import type { RecordRow } from '../composables/useRecords';

export interface SummaryB {
  count: number;
  open: number;
  closed: number;
  totalCents: number;
  byCategory: Record<string, number>;
}

export function summariseB(rows: RecordRow[]): SummaryB {
  const byCategory: Record<string, number> = {};
  let open = 0;
  let closed = 0;
  let totalCents = 0;

  for (const row of rows) {
    totalCents += row.amountCents;
    if (row.status === 'open') {
      open += 1;
    } else {
      closed += 1;
    }
    byCategory[row.category] = (byCategory[row.category] ?? 0) + 1;
  }

  return { count: rows.length, open, closed, totalCents, byCategory };
}
