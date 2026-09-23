import { scheduleSummary } from './roster-b';
import type { Member, ClassSession } from '../types';

export function rosterSummary(members: Member[]): { id: string; name: string }[] {
  return members.map((member) => ({ id: member.id, name: member.fullName }));
}

export function clubTotals(members: Member[], sessions: ClassSession[]) {
  return {
    members: rosterSummary(members).length,
    sessions: scheduleSummary(sessions).length,
  };
}
