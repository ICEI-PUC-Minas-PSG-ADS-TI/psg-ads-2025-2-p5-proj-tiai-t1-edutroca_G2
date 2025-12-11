export function Footer() {
  return (
    <footer className="bg-gray-50 border-t mt-auto">
      <div className="container mx-auto px-4 py-6 text-center text-gray-600">
        <p>
          &copy; {new Date().getFullYear()} EduTroca. Todos os direitos
          reservados.
        </p>
      </div>
    </footer>
  );
}
