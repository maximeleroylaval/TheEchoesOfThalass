using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Défi 6 - Nicolas Mori
/// Réécriture par Marc Lauzon.
/// </summary>

namespace Thalass {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class WildlifeController : MonoBehaviour {
        const float MAX_EVADING_RANGE = 1.0f;
        const float MAX_RETURN_RESET = 1.0f;

        [SerializeField]
        Entities.Wildlife m_wildlife = null;

        [Space]
        [SerializeField]
        float m_waterDrag = 0.1f;

        Rigidbody m_rigidbody = null;
        SphereCollider m_collider = null;
        Vector3 m_spawnPosition = Vector3.zero;

        float m_lastReaction = 0;
        Vector3 m_reactionSource = Vector3.zero;
        Reaction m_reaction = Reaction.Roaming;

        void Start() {
            //m_health = m_wildlife.Health;
            if (m_wildlife == null)
                return;

            m_spawnPosition = transform.position;
            GenerateTarget(transform.position);

            m_rigidbody = GetComponent<Rigidbody>();
            m_rigidbody.drag = m_waterDrag;
            m_rigidbody.angularDrag = m_waterDrag;

            m_collider = GetComponent<SphereCollider>();
            m_collider.radius = m_wildlife.ReactionRange;
        }

        void Update() {

            if (m_wildlife == null)
                return;

            switch (m_reaction) {
                //Idle, waiting for next target.
                case Reaction.None:
                    if (Time.timeSinceLevelLoad > m_lastReaction + m_wildlife.ReactionCooldown) {
                        m_reaction = Reaction.Roaming;

                        if (m_wildlife.TerritoryRange > 0)
                            GenerateTarget(m_spawnPosition, m_wildlife.TerritoryRange);
                        else
                            GenerateTarget(transform.position);
                    }
                    break;
                //Move toward target at cruse speed.
                case Reaction.Roaming:
                    if (Vector3.Distance(transform.position, m_reactionSource) < MAX_RETURN_RESET) {
                        m_lastReaction = Time.timeSinceLevelLoad;
                        m_reaction = Reaction.None;
                    } else {
                        MoveToward(m_reactionSource, m_wildlife.CrusingSpeed);
                    }
                    break;
                //Move opposite of target at reaction speed.
                case Reaction.Fleeing:
                    if (Time.timeSinceLevelLoad > m_lastReaction + m_wildlife.ReactionCooldown)
                        m_reaction = Reaction.None;
                    else {
                        MoveToward((transform.position - m_reactionSource) * 5, m_wildlife.ReactionSpeed);
                    }

                    break;
                //Move toward target at reaction speed.
                case Reaction.Attacking:
                    MoveToward(m_reactionSource, m_wildlife.ReactionSpeed);
                    break;
                //Move 
                case Reaction.Evade:
                    //TODO
                    break;
                //Don't react until time expires.
                case Reaction.Stunned:
                    if (Time.timeSinceLevelLoad > m_lastReaction + m_wildlife.ReactionCooldown)
                        m_reaction = Reaction.Attacking;
                    break;
                default:
                    break;
            }
        }

        void GenerateTarget(Vector3 _origin, float _distance = 10) {
            m_reactionSource = Vector3.Scale(_origin + Random.insideUnitSphere * _distance, new Vector3(1, .30f, 1));
            m_lastReaction = Time.timeSinceLevelLoad;
        }

        void MoveToward(Vector3 _target, float _speed) {
            Vector3 direction = (_target - transform.position).normalized;


            m_rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), (m_reaction == Reaction.Fleeing) ? .5f : 0.01f));


            //if (m_reaction == Reaction.Fleeing) m_rigidbody.AddExplosionForce(m_wildlife.ReactionSpeed, m_reactionSource, 10);
            m_rigidbody.AddForce(direction * _speed * Time.deltaTime, ForceMode.Impulse);
        }

        #region Trigger reactions
        void OnTriggerEnter(Collider other) {
            //If Submarine within range.
            if (other.GetComponent<SubmarineController>()) {
                if (m_wildlife.Comportement.HasFlag(Entities.Comportement.Aggressive)) {
                    //Is not stunned.
                    if (m_reaction != Reaction.Stunned) {
                        //Sub outside of 
                        if (m_wildlife.TerritoryRange > 0 && Vector3.Distance(other.transform.position, m_spawnPosition) > m_wildlife.TerritoryRange) {
                            m_reaction = Reaction.Roaming;
                            m_lastReaction = Time.timeSinceLevelLoad;
                        } else {
                            m_reaction = Reaction.Attacking;
                            m_reactionSource = other.transform.position;
                            m_lastReaction = Time.timeSinceLevelLoad;
                        }
                    }
                } else {
                    m_reaction = Reaction.Fleeing;
                    m_reactionSource = other.transform.position;
                    m_lastReaction = Time.timeSinceLevelLoad;
                }


            } else

            //If other wildlife within range.
            if (other.GetComponent<WildlifeController>()) {
                WildlifeController wildlifeCtrl = other.GetComponent<WildlifeController>();

                //Wildlife flee for safety as other wildlife flee or is aggressive.
                if (m_wildlife.Comportement.HasFlag(Entities.Comportement.Passive)
                    && (wildlifeCtrl.m_reaction.HasFlag(Reaction.Attacking)
                    || wildlifeCtrl.m_reaction.HasFlag(Reaction.Fleeing))) {

                    m_reaction = Reaction.Fleeing;
                    m_reactionSource = other.ClosestPoint(transform.position);
                }
            }
        }

        void OnTriggerStay(Collider other) {
            /*
            if (m_reaction.HasFlag(Reaction.Roaming)) {
                //Change direction.
                m_reactionSource = other.ClosestPoint(transform.position);
                if (Vector3.Distance(transform.position, m_reactionSource) < MAX_EVADING_RANGE)
                    m_reaction = Reaction.Evade;
            } else if (m_reaction.HasFlag(Reaction.Attacking)) {
                //TODO : ????
            }
            */
        }

        void OnTriggerExit(Collider other) {
            //Aggressive will return to origin.
            if ((m_wildlife.Comportement == Entities.Comportement.Aggressive) && other.GetComponent<SubmarineController>())
                m_reaction = Reaction.Roaming;
        }
        #endregion

        #region Collision reactions
        void OnCollisionEnter(Collision collision) {
            SubmarineController submarine = collision.gameObject.GetComponent<SubmarineController>();
            if (m_reaction.HasFlag(Reaction.Attacking) && submarine) {
                submarine.GetComponent<EntityController>().takeDamage(this.GetComponent<EntityController>().getDamage());
                m_lastReaction = Time.timeSinceLevelLoad;
                m_reaction = Reaction.Stunned;

                m_rigidbody.velocity *= 0.5f;
                m_rigidbody.angularVelocity *= 0f;
            }
        }
        #endregion
    }

    public enum Reaction {
        None = 0,
        Roaming,
        Fleeing,
        Attacking,
        Evade,
        Stunned,
    }
}
