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

const bookContainer = document.querySelector('#content');
const formContainer = document.querySelector('#form-container');

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

bookContainer.addEventListener('click', (e) => {
  if (e.target.id !== 'btn-delete') return;
  //deleteBook(e.target.)
  deleteBook(e.target.dataset.id);
});

// Funktioner
function deleteBook(id) {
  fetch(`${uri}/${id}`, {
    method: 'DELETE',
  })
    .then((res) => {
      if (res.status == 204) {
        getDataFromServer(uri);
      } else {
        throw new Error('Ett fel uppstod när boken skulle tas bort');
      }
    })
    .catch((error) => displayError(error));
}

function addBook(formData) {
  const book = {
    title: formData.get('title'),
    author: formData.get('author'),
    pageCount: formData.get('pagecount'),
    departement: formData.get('departement'),
    isbn: formData.get('isbn'),
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
        throw new Error('Ett fel uppstod när boken skulle läggas till');
      }
    })
    .then(() => {
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
    .then((response) => {
      if (response.status == 200) {
        return response.json();
      } else {
        throw new Error('Böckerna kunde inte hämtas från servern');
      }
    })
    .then((data) => displayAll(booksContainer, data))
    .catch((error) => displayError(error));
}

// Visa allt innehåll, formulär och listan med böcker
function displayAll(container, books) {
  addBookForm.classList.remove('was-validated');
  clearErrorMsg();
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
    <div class="card-header"><h5>Sammanfattning</h5></div>
    <div class="card-body">
      Samlingen består av ${numberOfBooks} böcker, sammanlagt ${totalPages} sidor.
    </div>
  </div>
  `;
  container.insertAdjacentHTML('afterbegin', html);
}

// Visar alla böcker
function displayBooks(container, books) {
  let html = '';
  books.sort((a, b) => {
    const authorA = a.author.split(' ');
    const authorB = b.author.split(' ');
    const lastNameA = authorA[authorA.length - 1].toUpperCase();
    const lastNameB = authorB[authorB.length - 1].toUpperCase();
    if (lastNameA < lastNameB) return -1;
    if (lastNameA > lastNameB) return 1;
    return 0;
  });

  books.forEach((book) => {
    html += `
        <div class="card text-dark bg-light mb-3">
          <div class="card-header">
            <h3 class="card-title">${book.title}</h3>
            <h5 class="card-text">${book.author}</h5>
          </div>
          <div class="card-body">
            <div>
              <span class="badge bg-secondary p-2">${book.departement}</span>
              <span class="badge bg-secondary p-2">${book.pageCount} sidor</span>
              <span class="badge bg-secondary p-2">ISBN: ${book.isbn}</span>
            </div>
            <div class="text-end">
            <button type="button" class="btn btn-secondary btn-sm" id="btn-update" data-id="${book.id}">Ändra</button>
              <button type="button" class="btn btn-danger btn-sm" id="btn-delete" data-id="${book.id}">Ta bort</button>
            </div>            
          </div>                      
        </div>
        `;
  });
  container.insertAdjacentHTML('afterbegin', html);
}
