/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{vue,ts}'],
  theme: {
    extend: {
      colors: {
        star: { 50: '#f5f7ff', 500: '#4f5bd5', 700: '#343d9c' },
      },
    },
  },
  plugins: [],
};
