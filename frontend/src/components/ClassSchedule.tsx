import { formatDateTime } from '../utils/format';
import type { ClassSession } from '../types';

interface Props {
  sessions: ClassSession[];
  showWaitlist?: boolean;
}

export default function ClassSchedule({ sessions, showWaitlist }: Props) {
  const maxVisible = 25;

  if (sessions.length === 0) {
    return <p>No classes scheduled.</p>;
  }

  return (
    <section>
      <h2>Schedule</h2>
      <ul>
        {sessions.map((session) => (
          <li key={session.id}>
            <strong>{session.title}</strong> with {session.instructor}
            {' '}&mdash; {formatDateTime(session.startsAt)}
            {' '}({session.remainingPlaces} of {session.capacity} places left)
          </li>
        ))}
      </ul>
    </section>
  );
}
