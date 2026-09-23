// Deliberate cycle: circular-a <-> circular-b (madge / dependency-cruiser signal).
import { labelB } from './circular-b';

export function labelA(depth: number): string {
  return depth <= 0 ? 'a' : `a>${labelB(depth - 1)}`;
}
