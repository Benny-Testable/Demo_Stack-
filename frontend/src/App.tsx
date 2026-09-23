import { useEffect, useState } from 'react';
import MemberRoster from './components/MemberRoster';
import ClassSchedule from './components/ClassSchedule';
import { fetchMembers, fetchClasses } from './api/client';
import type { Member, ClassSession } from './types';

export default function App() {
  const [members, setMembers] = useState<Member[]>([]);
  const [classes, setClasses] = useState<ClassSession[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    Promise.all([fetchMembers(), fetchClasses()])
      .then(([m, c]) => {
        setMembers(m);
        setClasses(c);
      })
      .catch((err: Error) => setError(err.message));
  }, []);

  if (error) {
    return <p role="alert">Could not load the club data: {error}</p>;
  }

  return (
    <main>
      <h1>Coastline Fitness</h1>
      <MemberRoster members={members} />
      <ClassSchedule sessions={classes} />
    </main>
  );
}
