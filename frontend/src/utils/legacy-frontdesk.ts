// Helpers from the front-desk terminal build. Nothing imports these any more.
import type { Member, Booking } from '../types';

export function padBadge(code: string, width = 10): string {
  return String(code).padEnd(width, ' ');
}

export function bandAttendance(percent: number): string {
  if (percent >= 85) return 'excellent';
  if (percent >= 60) return 'good';
  if (percent >= 35) return 'patchy';
  return 'lapsed';
}

export function countSuspended(members: Member[]): number {
  return members.filter((member) => member.isSuspended).length;
}

export function groupByState(bookings: Booking[]): Record<string, Booking[]> {
  return bookings.reduce<Record<string, Booking[]>>((groups, booking) => {
    groups[booking.state] = groups[booking.state] ?? [];
    groups[booking.state].push(booking);
    return groups;
  }, {});
}

// The front-desk terminal posted untyped JSON; this adapter has never been typed.
export function adaptTerminalPayload(payload: any): Record<string, unknown> {
  const normalised: Record<string, unknown> = {};
  for (const key of Object.keys(payload ?? {})) {
    normalised[key.toLowerCase()] = payload[key];
  }
  return normalised;
}

export function readLegacyField(payload: any, field: string): any {
  return payload?.[field] ?? null;
}
