// Helpers from the pre-React portfolio screens. Nothing imports these any more;
// they are kept while the finance export is rewritten.

export function padReference(reference, width = 12) {
  return String(reference).padEnd(width, ' ');
}

export function bandOccupancy(percent) {
  if (percent >= 90) return 'full';
  if (percent >= 65) return 'healthy';
  if (percent >= 40) return 'light';
  return 'vacant';
}

export function sumDeposits(leases) {
  return leases.reduce((total, lease) => total + (lease.depositHeld ?? 0), 0);
}

export function groupByCity(properties) {
  return properties.reduce((groups, property) => {
    const key = property.city ?? 'unknown';
    groups[key] = groups[key] ?? [];
    groups[key].push(property);
    return groups;
  }, {});
}
