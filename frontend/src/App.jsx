import { useEffect, useState } from 'react';
import ProgrammeList from './components/ProgrammeList.jsx';
import ApplicationTable from './components/ApplicationTable.jsx';
import { fetchProgrammes, fetchApplications } from './api/client.js';

export default function App() {
  const [programmes, setProgrammes] = useState([]);
  const [applications, setApplications] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    Promise.all([fetchProgrammes(), fetchApplications()])
      .then(([p, a]) => {
        setProgrammes(p);
        setApplications(a);
      })
      .catch((err) => setError(err.message));
  }, []);

  if (error) {
    return <p role="alert">Could not load the awards data: {error}</p>;
  }

  return (
    <main>
      <h1>Scholarship Awards</h1>
      <ProgrammeList programmes={programmes} />
      <ApplicationTable applications={applications} />
    </main>
  );
}
