import { describe, expect, it } from 'vitest';
import { formatCurrency, formatDate, slugify, truncate } from '../src/utils/format';

describe('format', () => {
  it('formats cents as currency', () => {
    expect(formatCurrency(125000)).toBe('$1,250.00');
  });

  it('formats an ISO date', () => {
    expect(formatDate('2026-09-23T09:00:00Z')).toBe('2026-09-23');
  });

  it('returns a dash for an invalid date', () => {
    expect(formatDate('not-a-date')).toBe('—');
  });

  it('truncates long strings only', () => {
    expect(truncate('short', 10)).toBe('short');
    expect(truncate('a'.repeat(50), 10)).toHaveLength(10);
  });

  it('slugifies tenant names', () => {
    expect(slugify('  Star Alpha Ltd ')).toBe('star_alpha_ltd');
  });
});
