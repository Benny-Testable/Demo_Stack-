export interface Member {
  id: string;
  fullName: string;
  email: string;
  tier: MembershipTier;
  joinedOn: string;
  isSuspended: boolean;
  bookingCount: number;
}

export interface ClassSession {
  id: string;
  title: string;
  instructor: string;
  startsAt: string;
  durationMinutes: number;
  capacity: number;
  remainingPlaces: number;
  minimumTier: MembershipTier;
}

export interface Booking {
  id: string;
  memberId: string;
  classSessionId: string;
  bookedAt: string;
  state: BookingState;
}

export type MembershipTier = 'Flex' | 'Standard' | 'Premium' | 'Elite';
export type BookingState = 'Reserved' | 'Attended' | 'Cancelled' | 'NoShow';
