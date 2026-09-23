// Nothing imports these: dead-code / unused-export signal (knip, ts-unused-exports).
export const UNUSED_FEATURE_FLAG = 'star.legacy.export';

export function unusedHelper(value: string): string {
  return value.split('').reverse().join('');
}

export interface UnusedShape {
  id: string;
  retiredAt: string;
}
