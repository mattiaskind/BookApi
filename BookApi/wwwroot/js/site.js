'use strict';

// Sample data
let books = [
  {
    title: 'En titel på en bok',
    author: 'Gabriel García Márquez',
    pageCount: 430,
    departement: 'Skönlitteratur',
    ISBN: '9789146236108',
  },
  {
    title: 'En annan bok',
    author: 'Gabriel García Márquez',
    pageCount: 430,
    departement: 'Skönlitteratur',
    ISBN: '9789146236108',
  },
];

// Globals
const uri = 'books';
const booksContainer = document.querySelector('.books');
const addBookBtn = document.querySelector('#add-book-btn');
const addBookForm = document.querySelector('#add-book-form');

getDataFromServer(uri);
//displayAll(booksContainer, books);

// Event listeners
addBookBtn.addEventListener('click', (e) => {
  e.preventDefault();
  const formData = new FormData(addBookForm);
  for (var value of formData.values()) {
    if (value === '') return;
  }
  addBook(formData);
});

// Funktioner
function addBook(formData) {
  const book = {
    title: formData.get('title'),
    author: formData.get('author'),
    pageCount: formData.get('pagecount'),
    departement: formData.get('departement'),
    ISBN: formData.get('isbn'),
  };

  fetch(uri, {
    method: 'POST',
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(book),
  })
    .then((response) => response.json())
    .then(() => {
      getDataFromServer(uri);
    })
    .catch((error) => console.error('Unable to add item.', error));
}

function getDataFromServer(uri) {
  fetch(uri)
    .then((response) => response.json())
    .then((data) => displayAll(booksContainer, data))
    .catch((error) => console.error('Unable to get items.', error));
}

// Visa allt innehåll, formulär och listan med böcker
function displayAll(container, books) {
  container.innerHTML = '';
  displayBooks(container, books);
  displaySummary(container, books);
}

// Visar summering
function displaySummary(container, books) {
  let html = '';
  const numberOfBooks = books.length;
  const totalPages = books.reduce((prev, curr) => {
    return prev + curr.pageCount;
  }, 0);

  html += `
  <div class="books-summary">
    <div>Samlingen består av ${numberOfBooks} böcker</div>
    <div>Tillsammans består böckerna av ${totalPages} sidor</div>  
  </div>
  `;
  container.insertAdjacentHTML('afterbegin', html);
}

// Visar alla böcker
function displayBooks(container, books) {
  let html = '';
  books.forEach((book) => {
    html += `
        <div class="book">
            <div><h3 class="book-title">${book.title}</h3></div>
            <div class="book-author">${book.author}</div>
            <div class="book-info">
                <ul>
                    <li>${book.departement}</li>
                    <li>${book.pageCount} sidor</li>
                    <li>ISBN: ${book.isbn}</li>
                </ul>
            </div>            
        </div>
        `;
  });
  container.insertAdjacentHTML('afterbegin', html);
}
