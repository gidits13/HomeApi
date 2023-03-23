using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _repository;
        private IMapper _mapper;
        
        public RoomsController(IRoomRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        /// <summary>
        /// Обновление комнаты
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPut]
        public async Task<IActionResult> UpdateRoom([FromBody] UpdateRoomRequest request)
        {
            var room = await _repository.GetRoomById(request.Guid);
            if (room is null)
                return StatusCode(401, $"Комната не найдена ({request.Guid})");
           
            room.Name = request.Name;
            room.Area = request.Area;
            room.Voltage = request.Voltage;
            room.GasConnected = request.GasConnected;
            
            await _repository.UpdateRoom(room);
            return StatusCode(201, "Комната успешно обновлена");


        }
        //TODO: Задание - добавить метод на получение всех существующих комнат
        /// <summary>
        /// получение всех комнат
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _repository.GetRooms();
            var resp = new GetRoomsResponse
            {
                RoomAmount = rooms.Length,
                Rooms = _mapper.Map<Room[], RoomView[]>(rooms)
            };
            return StatusCode(200, resp);
        }
        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost] 
        [Route("")] 
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _repository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }
            
            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }
    }
}