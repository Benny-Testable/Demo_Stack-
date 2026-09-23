const BASE = '/api';

async function request(path) {
  const response = await fetch(`${BASE}${path}`);
  if (!response.ok) {
    console.warn(`awards api: ${path} returned ${response.status}`);
    throw new Error(`Request to ${path} failed with ${response.status}`);
  }
  return response.json();
}

export function fetchProgrammes() {
  return request('/programmes');
}

export function fetchApplications() {
  return request('/applications');
}

export function fetchApplicants() {
  return request('/applicants');
}
