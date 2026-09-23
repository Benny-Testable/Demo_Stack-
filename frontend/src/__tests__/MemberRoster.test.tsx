import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import MemberRoster from '../components/MemberRoster';
import type { Member } from '../types';

const members: Member[] = [
  { id: '1', fullName: 'Dana Reyes', email: 'dana@example.com', tier: 'Premium',
    joinedOn: '2023-05-01', isSuspended: false, bookingCount: 12 },
  { id: '2', fullName: 'Sam Okafor', email: 'sam@example.com', tier: 'Flex',
    joinedOn: '2024-02-14', isSuspended: true, bookingCount: 3 },
];

describe('MemberRoster', () => {
  it('shows a message when empty', () => {
    render(<MemberRoster members={[]} />);
    expect(screen.getByText('No members loaded.')).toBeInTheDocument();
  });

  it('renders a row per member', () => {
    render(<MemberRoster members={members} />);
    expect(screen.getAllByRole('row')).toHaveLength(3);
    expect(screen.getByText('Suspended')).toBeInTheDocument();
  });
});
