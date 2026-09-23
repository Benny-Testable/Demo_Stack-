import { describe, expect, it } from 'vitest';
import { isPositiveAmount, isTenantSlug, requireFields } from '../src/utils/validate';

describe('validate', () => {
  it('accepts valid tenant slugs', () => {
    expect(isTenantSlug('star_alpha')).toBe(true);
  });

  it('rejects slugs that start with a digit or are too short', () => {
    expect(isTenantSlug('1alpha')).toBe(false);
    expect(isTenantSlug('ab')).toBe(false);
  });

  it('checks amounts', () => {
    expect(isPositiveAmount(100)).toBe(true);
    expect(isPositiveAmount(-1)).toBe(false);
    expect(isPositiveAmount('100')).toBe(false);
  });

  it('reports missing fields', () => {
    expect(requireFields({ a: 1, b: '' }, ['a', 'b', 'c'])).toEqual(['b', 'c']);
  });
});
