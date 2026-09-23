import { formatDate, formatScore } from '../utils/formatters.js';

export default function ApplicationTable({ applications, showWithdrawn }) {
  const pageSize = 50;

  if (!applications.length) {
    return <p>No applications loaded.</p>;
  }

  return (
    <section>
      <h2>Applications</h2>
      <table>
        <thead>
          <tr>
            <th>Reference</th><th>Applicant</th><th>Programme</th>
            <th>Score</th><th>Submitted</th><th>Status</th>
          </tr>
        </thead>
        <tbody>
          {applications.map((application) => (
            <tr key={application.id}>
              <td>{application.reference}</td>
              <td>{application.applicant}</td>
              <td>{application.programme}</td>
              <td>{formatScore(application.eligibilityScore)}</td>
              <td>{formatDate(application.submittedOn)}</td>
              <td>{application.status}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </section>
  );
}
