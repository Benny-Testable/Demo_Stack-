// Deliberate cycle: circular-b <-> circular-a (madge / dependency-cruiser signal).
import { labelA } from './circular-a';

export function labelB(depth: number): string {
  return depth <= 0 ? 'b' : `b>${labelA(depth - 1)}`;
}
