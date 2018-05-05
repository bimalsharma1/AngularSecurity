using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PtcApi.Model;
using Microsoft.AspNetCore.Authorization;

namespace PtcApi.Controllers
{
  [Route("api/Menu/")]
  // [Authorize]
  public class MenuController : BaseApiController
  {
    [HttpGet]
    // [Authorize(Policy = "CanAccessProducts")]
    public IActionResult GetMenu()
    {
      IActionResult ret = null;
      List<Menu> list = new List<Menu>();

      try
      {
        using (var db = new PtcDbContext())
        {
          if (db.Menus.Count() > 0)
          {
            list = db.Menus.OrderBy(p => p.MenuName).ToList();
            ret = StatusCode(StatusCodes.Status200OK, list);
          }
          else
          {
            ret = StatusCode(StatusCodes.Status404NotFound, "Can't Find Menus");
          }
        }
      }
      catch (Exception ex)
      {
        ret = HandleException(ex, "Exception trying to get all Menus");
      }

      return ret;
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      IActionResult ret = null;
      Menu entity = null;

      try
      {
        using (var db = new PtcDbContext())
        {
          entity = db.Menus.Find(id);
          if (entity != null)
          {
            ret = StatusCode(StatusCodes.Status200OK, entity);
          }
          else
          {
            ret = StatusCode(StatusCodes.Status404NotFound,
                     "Can't Find Menu: " + id.ToString());
          }
        }
      }
      catch (Exception ex)
      {
        ret = HandleException(ex,
          "Exception trying to retrieve a single Menu.");
      }

      return ret;
    }

    [HttpPost()]
    public IActionResult Post([FromBody]Menu entity)
    {
      IActionResult ret = null;

      try
      {
        using (var db = new PtcDbContext())
        {
          if (entity != null)
          {
            db.Menus.Add(entity);
            db.SaveChanges();
            ret = StatusCode(StatusCodes.Status201Created,
                entity);
          }
          else
          {
            ret = StatusCode(StatusCodes.Status400BadRequest, "Invalid object passed to POST method");
          }
        }
      }
      catch (Exception ex)
      {
        ret = HandleException(ex, "Exception trying to insert a new Menu");
      }

      return ret;
    }

    [HttpPut()]
    public IActionResult Put([FromBody]Menu entity)
    {
      IActionResult ret = null;

      try
      {
        using (var db = new PtcDbContext())
        {
          if (entity != null)
          {
            db.Update(entity);
            db.SaveChanges();
            ret = StatusCode(StatusCodes.Status200OK, entity);
          }
          else
          {
            ret = StatusCode(StatusCodes.Status400BadRequest, "Invalid object passed to PUT method");
          }
        }
      }
      catch (Exception ex)
      {
        ret = HandleException(ex, "Exception trying to update Menu: " + entity.MenuId.ToString());
      }

      return ret;
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      IActionResult ret = null;
      Menu entity = null;

      try
      {
        using (var db = new PtcDbContext())
        {
          entity = db.Menus.Find(id);
          if (entity != null)
          {
            db.Menus.Remove(entity);
            db.SaveChanges();
          }
          ret = StatusCode(StatusCodes.Status200OK, true);
        }
      }
      catch (Exception ex)
      {
        ret = HandleException(ex, "Exception trying to delete Menu: " + id.ToString());
      }

      return ret;
    }
  }
}
