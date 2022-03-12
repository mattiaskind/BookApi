'use strict';

// Globals
const uri = 'books';

//const addBookBtn = document.querySelector('#add-book-btn');
//const formFields = document.querySelectorAll('input');

const booksContainer = document.querySelector('#content');
const formContainer = document.querySelector('#form-container');

const messageCard = document.querySelector('#message-card');
const messageParagraph = document.querySelector('#message-p');

displayAddBookForm();
getBooks(uri);

//-------------------- Event Listeners --------------------

// Listan - Ta bort en bok
booksContainer.addEventListener('click', (e) => {
  if (e.target.id !== 'btn-delete') return;
  e.preventDefault();
  deleteBook(e.target.dataset.id);
});

// Listan - Ändra en bok
booksContainer.addEventListener('click', (e) => {
  if (e.target.id !== 'btn-update') return;
  e.preventDefault();
  displayUpdateForm(e.target.dataset.id);
});

// Formuläret - Lägg till en ny bok
formContainer.addEventListener('click', (e) => {
  if (e.target.id !== 'add-book-btn') return;
  e.preventDefault();
  const form = document.querySelector('#add-book-form');
  const formData = new FormData(form);
  if (!form.checkValidity()) {
    displayMessage('Samtliga fält är obligatoriska');
    return;
  }
  hideMessageCard();
  addBook(formData);
});

// Formuläret - Ångra ändring
formContainer.addEventListener('click', (e) => {
  if (e.target.id !== 'undo-btn') return;
  e.preventDefault();
  displayAddBookForm();
});

// Formuläret - Ändra en bok
formContainer.addEventListener('click', (e) => {
  if (e.target.id !== 'update-book-btn') return;
  e.preventDefault();
  const form = document.querySelector('#update-book-form');
  const id = document.querySelector('#update-book-btn').getAttribute('data-id');
  const formData = new FormData(form);
  if (!form.checkValidity()) {
    displayMessage('Samtliga fält är obligatoriska');
    return;
  }
  hideMessageCard();
  updateBook(id, formData);
});

// -------------------- Funktioner - API-anrop --------------------

async function getBooks(uri) {
  try {
    const response = await fetch(uri);
    if (response.status === 404) {
      displayBooks(booksContainer, null);
      throw 'Samlingen innehåller för tillfället inga böcker';
    }
    if (response.status !== 200) throw 'Det gick inte att hämta böcker från servern';

    const json = await response.json();
    resetForm();
    displayBooks(booksContainer, json);
  } catch (error) {
    displayMessage(error);
  }
}

async function deleteBook(id) {
  try {
    const response = await fetch(`${uri}/${id}`, { method: 'DELETE' });
    if (response.status !== 204) throw 'Ett fel uppstod när boken skulle tas bort';
    getBooks(uri);
  } catch (error) {
    displayMessage(error);
  }
}

async function updateBook(id, formData) {
  const book = {
    title: formData.get('title'),
    author: formData.get('author'),
    pageCount: parseInt(formData.get('pagecount')),
    departement: formData.get('departement'),
    isbn: formData.get('isbn'),
  };

  try {
    const response = await fetch(`${uri}/${id}`, {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(book),
    });
    if (response.status !== 204) {
      const json = await response.json();
      let errorMessage = `
          <ul class="list-group">
          `;
      for (let [_, value] of Object.entries(json.errors)) {
        errorMessage += `<li class="list-group-item list-group-item-warning">${value[0]}</li>`;
      }
      errorMessage += `
          </ul>`;
      throw errorMessage;
    } else {
      displayAddBookForm();
      getBooks(uri);
    }
  } catch (error) {
    displayMessage(error);
  }
}

async function addBook(formData) {
  const book = {
    title: formData.get('title'),
    author: formData.get('author'),
    pageCount: formData.get('pagecount'),
    departement: formData.get('departement'),
    isbn: formData.get('isbn'),
  };

  try {
    const response = await fetch(uri, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(book),
    });

    if (response.status !== 201) {
      const json = await response.json();
      let errorMessage = `
          <ul class="list-group">
          `;
      for (let [_, value] of Object.entries(json.errors)) {
        errorMessage += `<li class="list-group-item list-group-item-warning">${value[0]}</li>`;
      }
      errorMessage += `
          </ul>`;
      throw errorMessage;
    } else {
      getBooks(uri);
    }
  } catch (error) {
    displayMessage(error);
  }
}

// -------------------- Funktioner - Gränssnitt --------------------

function displayMessage(msg) {
  messageCard.classList.remove('invisible');
  messageParagraph.innerHTML = msg;
}

function hideMessageCard() {
  messageParagraph.innerText = '';
  messageCard.classList.add('invisible');
}

function resetForm() {
  const inputs = document.querySelectorAll('input');
  inputs.forEach((input) => (input.value = ''));
}

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
      Samlingen består av ${numberOfBooks} ${numberOfBooks === 1 ? 'bok' : 'böcker'}, sammanlagt ${totalPages} sidor.
    </div>
  </div>
  `;
  container.insertAdjacentHTML('afterbegin', html);
}

// Visar alla böcker
function displayBooks(container, books) {
  container.innerHTML = '';
  if (books === null) return;

  displaySummary(container, books);

  let html = '';
  // Primitiv sortering på efternamn
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
  container.insertAdjacentHTML('beforeend', html);
}

function displayAddBookForm() {
  formContainer.innerHTML = '';
  const html = `
  <form id="add-book-form">
    <div class="mb-2">
      <label for="title" class="form-label">Titel</label>
      <input type="text" name="title" id="title" class="form-control" required />
    </div>
    <div class="mb-2">
      <label for="title" class="form-label">Författare</label>
      <input type="text" name="author" id="author" class="form-control" required />
    </div>
    <div class="mb-2">
      <label for="title" class="form-label">Avdelning (exempelvis skönlitteratur)</label>
      <input type="text" name="departement" id="departement" class="form-control" required />
    </div>
    <div class="mb-2">
      <label for="title" class="form-label">Antal sidor</label>
      <input type="number" name="pagecount" id="pagecount" class="form-control" required />
    </div>
    <div class="mb-3">
      <label for="title" class="form-label">ISBN (måste vara mellen 10 och 13 siffror)</label>
      <input type="number" name="isbn" id="isbn" class="form-control" required />
    </div>
    <div class="mb-3">       
      <button type="button" class="btn btn-primary" id="add-book-btn">Lägg till bok</button>
    </div>
  </form>
  `;
  formContainer.insertAdjacentHTML('afterbegin', html);
}
async function displayUpdateForm(id) {
  formContainer.innerHTML = '';
  hideMessageCard();
  let data = null;
  try {
    const response = await fetch(`${uri}/${id}`);
    if (response.status !== 200) throw 'Kunde inte hämta information om boken';
    data = await response.json();
  } catch (error) {
    displayAddBookForm();
    displayMessage(error);
    return;
  }

  const html = `
  <form id="update-book-form">
    <div class="mb-2">
      <label for="title" class="form-label">Titel</label>
      <input type="text" name="title" id="title" class="form-control" value="${data.title}" required />
    </div>
    <div class="mb-2">
      <label for="title" class="form-label">Författare</label>
      <input type="text" name="author" id="author" class="form-control" value="${data.author}" required />
    </div>
    <div class="mb-2">
      <label for="title" class="form-label">Avdelning (exempelvis skönlitteratur)</label>
      <input type="text" name="departement" id="departement" class="form-control" value="${data.departement}" required />
    </div>
    <div class="mb-2">
      <label for="title" class="form-label">Antal sidor</label>
      <input type="number" name="pagecount" id="pagecount" class="form-control" value="${data.pageCount}" required />
    </div>
    <div class="mb-3">
      <label for="title" class="form-label">ISBN (måste vara mellen 10 och 13 siffror)</label>
      <input type="number" name="isbn" id="isbn" class="form-control" value="${data.isbn}" required />
    </div>
    <div class="mb-3">       
      <button type="button" class="btn btn-secondary" id="undo-btn">Ångra</button>
      <button type="button" class="btn btn-primary" id="update-book-btn" data-id="${data.id}">Ändra</button>
    </div>
  </form>
  `;
  formContainer.insertAdjacentHTML('afterbegin', html);
}
