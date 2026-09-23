import type { RecordRow } from '../composables/useRecords';

function build(prefix: string, seed: number): RecordRow[] {
  const categories = ['billing', 'support', 'onboarding', 'compliance'];
  return Array.from({ length: 12 }, (_, i) => ({
    id: seed * 100 + i + 1,
    title: `${prefix} record #${String(i + 1).padStart(2, '0')}`,
    category: categories[(i + seed) % categories.length],
    amountCents: (i + 1) * 1250,
    status: (i + 1) % 4 === 0 ? 'closed' : 'open',
    createdAt: `2026-09-${String((i % 27) + 1).padStart(2, '0')}T09:00:00Z`,
  }));
}

export const SAMPLE_RECORDS: Record<string, RecordRow[]> = {
  star_alpha: build('Alpha', 1),
  star_beta: build('Beta', 2),
  star_gamma: build('Gamma', 3),
};
