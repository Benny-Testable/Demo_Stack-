import { formatCurrency } from '../utils/formatters.js';

export default function ProgrammeList({ programmes }) {
  if (!programmes.length) {
    return <p>No programmes loaded.</p>;
  }

  return (
    <section>
      <h2>Programmes</h2>
      <ul>
        {programmes.map((programme) => (
          <li key={programme.id}>
            <strong>{programme.code}</strong> {programme.name} &mdash; {programme.category}
            {' '}({programme.placesAvailable} places at {formatCurrency(programme.awardAmount)})
          </li>
        ))}
      </ul>
    </section>
  );
}
