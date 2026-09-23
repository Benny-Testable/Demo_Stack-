import type { Member, ClassSession, Booking } from '../types';

const BASE = '/api';

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${BASE}${path}`, init);
  if (!response.ok) {
    console.warn(`coastline api: ${path} returned ${response.status}`);
    throw new Error(`Request to ${path} failed with ${response.status}`);
  }
  return (await response.json()) as T;
}

export function fetchMembers(): Promise<Member[]> {
  return request<Member[]>('/members');
}

export function fetchClasses(): Promise<ClassSession[]> {
  return request<ClassSession[]>('/classes');
}

export function bookClass(sessionId: string, memberId: string): Promise<Booking> {
  return request<Booking>(`/classes/${sessionId}/bookings?memberId=${memberId}`, {
    method: 'POST',
  });
}
