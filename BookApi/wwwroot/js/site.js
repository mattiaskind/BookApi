'use strict';

// Sample data
// let books = [
//   {
//     title: 'En titel på en bok',
//     author: 'Gabriel García Márquez',
//     pageCount: 430,
//     departement: 'Skönlitteratur',
//     isbn: '9789146236108',
//   },
//   {
//     title: 'En annan bok',
//     author: 'Gabriel García Márquez',
//     pageCount: 430,
//     departement: 'Skönlitteratur',
//     isbn: '9789146236108',
//   },
// ];

// Globals
const uri = 'books';
const booksContainer = document.querySelector('#content');
const addBookBtn = document.querySelector('#add-book-btn');
const addBookForm = document.querySelector('#add-book-form');
const formFields = document.querySelectorAll('input');

const errorCard = document.querySelector('#error-card');
const errorMsgParagraph = document.querySelector('#error-msg');

getDataFromServer(uri);
//displayAll(booksContainer, books);

// Event listeners
addBookForm.addEventListener('submit', (e) => {
  e.preventDefault();
  const formData = new FormData(addBookForm);
  addBookForm.classList.add('was-validated');
  if (!addBookForm.checkValidity()) return;
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
    .then((response) => {
      if (response.status == 201) {
        return response.json();
      } else {
        console.log('ERRROR');
        throw new Error('Ett fel uppstod när boken skulle läggas till');
      }
    })
    .then(() => {
      console.log('TRYING');
      getDataFromServer(uri);
    })
    .catch((error) => {
      displayError(error);
    });
}

function displayError(msg) {
  errorMsgParagraph.innerText = msg;
  errorCard.classList.remove('invisible');
}

function clearErrorMsg() {
  errorMsgParagraph.innerText = '';
  errorCard.classList.add('invisible');
}

function getDataFromServer(uri) {
  fetch(uri)
    .then((response) => response.json())
    .then((data) => displayAll(booksContainer, data))
    .catch((error) => console.error('Kunde inte hämta några böcker.', error));
}

// Visa allt innehåll, formulär och listan med böcker
function displayAll(container, books) {
  clearErrorMsg();
  addBookForm.classList.remove('was-validated');
  container.innerHTML = '';
  displayBooks(container, books);
  displaySummary(container, books);
  formFields.forEach((element) => {
    element.value = '';
  });
}

// Visar summering
function displaySummary(container, books) {
  let html = '';
  const numberOfBooks = books.length;
  const totalPages = books.reduce((prev, curr) => {
    return prev + curr.pageCount;
  }, 0);

  html += `
  <div class="card text-dark bg-light mb-3">
    <div class="card-header">Sammanfattning</div>
    <div class="card-body">
      Samlingen består av ${numberOfBooks} böcker som totalt
      uppgår till ${totalPages} sidor.
    </div>
  </div>
  `;
  container.insertAdjacentHTML('afterbegin', html);
}

// Visar alla böcker
function displayBooks(container, books) {
  let html = '';
  books.forEach((book) => {
    html += `
        <div class="card text-dark bg-light mb-3">
          <div class="card-header">
            <h3 class="card-title">${book.title}</h3>
            <h5 class="card-text">${book.author}</h5>
          </div>
          <div class="card-body">
            <span class="badge bg-secondary p-2">${book.departement}</span>
            <span class="badge bg-secondary p-2">${book.pageCount} sidor</span>
            <span class="badge bg-secondary p-2">ISBN: ${book.isbn}</span>          
          </div>                      
        </div>
        `;
  });
  container.insertAdjacentHTML('afterbegin', html);
}
