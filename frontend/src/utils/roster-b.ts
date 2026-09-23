import { rosterSummary } from './roster-a';
import type { Member, ClassSession } from '../types';

export function scheduleSummary(sessions: ClassSession[]): { id: string; title: string }[] {
  return sessions.map((session) => ({ id: session.id, title: session.title }));
}

export function sessionsPerMember(members: Member[], sessions: ClassSession[]): number {
  const count = rosterSummary(members).length;
  return count === 0 ? 0 : sessions.length / count;
}
