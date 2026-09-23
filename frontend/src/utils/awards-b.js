import { summariseProgrammes } from './awards-a.js';

export function summariseApplications(applications) {
  return applications.map((application) => ({
    id: application.id,
    reference: application.reference,
    score: application.eligibilityScore ?? 0,
  }));
}

export function applicationsPerProgramme(programmes, applications) {
  const count = summariseProgrammes(programmes).length;
  return count === 0 ? 0 : applications.length / count;
}
