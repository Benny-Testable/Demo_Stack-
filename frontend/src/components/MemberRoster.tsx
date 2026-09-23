import { formatDate, tierLabel } from '../utils/format';
import type { Member } from '../types';

interface Props {
  members: Member[];
}

export default function MemberRoster({ members }: Props) {
  if (members.length === 0) {
    return <p>No members loaded.</p>;
  }

  return (
    <section>
      <h2>Members</h2>
      <table>
        <thead>
          <tr>
            <th>Name</th><th>Tier</th><th>Joined</th><th>Bookings</th><th>Status</th>
          </tr>
        </thead>
        <tbody>
          {members.map((member) => (
            <tr key={member.id}>
              <td>{member.fullName}</td>
              <td>{tierLabel(member.tier)}</td>
              <td>{formatDate(member.joinedOn)}</td>
              <td>{member.bookingCount}</td>
              <td>{member.isSuspended ? 'Suspended' : 'Active'}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </section>
  );
}
