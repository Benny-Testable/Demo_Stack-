// Helpers from the committee's spreadsheet tooling. Nothing imports these now.

export function padReference(reference, width = 16) {
  return String(reference).padEnd(width, ' ');
}

export function bandScore(score) {
  if (score >= 85) return 'outstanding';
  if (score >= 65) return 'strong';
  if (score >= 50) return 'borderline';
  return 'below threshold';
}

export function sumAwarded(applications) {
  return applications
    .filter((application) => application.status === 'Awarded')
    .reduce((total, application) => total + (application.awardAmount ?? 0), 0);
}

export function groupByInstitution(applicants) {
  return applicants.reduce((groups, applicant) => {
    const key = applicant.institution ?? 'unknown';
    groups[key] = groups[key] ?? [];
    groups[key].push(applicant);
    return groups;
  }, {});
}
