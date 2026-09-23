import React, { useEffect, useState } from 'react';
import PropertyList from './components/PropertyList.jsx';
import LeaseTable from './components/LeaseTable.jsx';
import { fetchProperties, fetchLeases } from './api/client.js';

export default function App() {
  const [properties, setProperties] = useState([]);
  const [leases, setLeases] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    Promise.all([fetchProperties(), fetchLeases()])
      .then(([p, l]) => {
        setProperties(p);
        setLeases(l);
      })
      .catch((err) => setError(err.message));
  }, []);

  if (error) {
    return <p role="alert">Could not load the portfolio: {error}</p>;
  }

  return (
    <main>
      <h1>Estates Portfolio</h1>
      <PropertyList properties={properties} />
      <LeaseTable leases={leases} />
    </main>
  );
}
