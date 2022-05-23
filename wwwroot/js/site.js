// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let articulosCarrito = [];

function verId(id){
      

    var url = `https://localhost:5001/api/carrito?id=${id}`;

    fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => response.json())
    .then(data => {

        const {course} = data;
        console.log(course);
       

       const ver = articulosCarrito.find(prod => prod.id === course.id);

      if(ver == null){

        articulosCarrito = [...articulosCarrito , course];
       
      }

      carritoHtml();

    })
    .catch(error => console.log(error));

}

 const carrito = document.querySelector('#carrito');
 const lista = document.querySelector("#lista-cursos");
 const contenedor = document.querySelector('#body');
 
 
 

 cargarEventListeners();
 function cargarEventListeners(){

    document.addEventListener('DOMContentLoaded' , () => {
    
        articulosCarrito = JSON.parse(localStorage.getItem('carrito')) || [];
        carritoHtml();

         const url = window.location.href;

        if(url == 'https://localhost:5001/Pago/RealizarPago'){
             
            if(articulosCarrito.length == 0){
                window.location.href = 'https://localhost:5001/Carro/MostrarItems';
                
            }

            const monto = document.getElementById('monto');
            monto.value = localStorage.getItem('total');
        }

       

    });
  
    
    carrito.addEventListener('click' , eliminarCurso);

 }

 function vaciarCarrito(){

     articulosCarrito.splice(0 , articulosCarrito.length);
     carritoHtml();

 }

 function eliminarCurso(e){
   
     if(e.target.classList.contains('borrar-curso')){
  
        const id = e.target.getAttribute('data-id');
        const indice = articulosCarrito.findIndex( prod => prod.id === id);
        articulosCarrito.splice(indice , 1);
        carritoHtml();

        var URLactual = window.location.href;
       
        if(URLactual == 'https://localhost:5001/Carro/MostrarItems'){
            mostrarElementos(articulosCarrito);
        }
    
     }

 }




 // Lee el contenido del html y extrae la informacion del curso //



  // Muestra el carrito en el html //

  function carritoHtml(){
    
    limpiarCarrito();
    articulosCarrito.forEach(curso => {
      
        const row = document.createElement('tr');
    
         row.innerHTML = `

            <td>
              
            <img src="data:image/png;base64,${curso.fileBase64}" width="100" >
             
            </td>
           
             <td>
              
                 ${curso.nombre}

             </td>

             <td>
              
                 ${curso.precio}

             </td>

             <td>
              
               ${curso.fechaInicio}

         </td>

         <td>
              
             <a class="borrar-curso" data-id="${curso.id}">x</a>

         </td>

         `;
    
         contenedor.appendChild(row);
   
    })

    // Sincronizar con el storage //

        localStorage.setItem('carrito' , JSON.stringify(articulosCarrito));    

  }

  function limpiarCarrito(){

      contenedor.innerHTML = '';
  }